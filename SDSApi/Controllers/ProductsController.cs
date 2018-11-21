using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using SDSApi.Models;

namespace SDSApi.Controllers
{
    public class ProductsController : ApiController
    {
        private DBModel db = new DBModel();
        private bool val;

        // GET: api/Products

        public IEnumerable<Product> GetCustomer([FromUri]PagingParameterModel pagingparametermodel)
        {
            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            string SortOrder = pagingparametermodel.sortOrder;

            string ColumnName = pagingparametermodel.columnName;
            //WHAT FIELD IS BEING SEARCHED
            string QuerySearchName = pagingparametermodel.querySearchName;    //.ToLower();

            var source = Enumerable.Empty<Product>().AsQueryable();

            var propertyInfo = typeof(Product).GetProperty(ColumnName);
            if (SortOrder == "desc")
            {
                source = (
                from product in db.Products.AsEnumerable().OrderByDescending(a =>
                    propertyInfo.GetValue(a, null)
                )
                select product
            ).AsQueryable();
            }
            else
            {
                source = (
                from product in db.Products.AsEnumerable().OrderBy(a =>
                    propertyInfo.GetValue(a, null)
                )
                select product
            ).AsQueryable();
            }


            if (!string.IsNullOrEmpty(pagingparametermodel.querySearch)){
                var propertyQueryInfo = typeof(Product).GetProperty(pagingparametermodel.querySearchName);

                source = (source.AsEnumerable().Where(
                    a => propertyQueryInfo.GetValue(a, null).ToString().ToLower().Contains(pagingparametermodel.querySearch.ToLower())
              
                )).AsQueryable();
        }


            //int count = db.Products.Count();
            int count = source.Count();

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = pagingparametermodel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pagingparametermodel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                // Object which we are going to send in header   
                var paginationMetadata = new
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage,
                    nextPage
                };

                // Setting Header  
                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
                // Returing List of Details
                return items;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProducts(int id)
        {
            Product products = await db.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProducts(int id, Product products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != products.id)
            {
                return BadRequest();
            }

            db.Entry(products).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProducts(Product products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(products);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = products.id }, products);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProducts(int id)
        {
            Product products = await db.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            db.Products.Remove(products);
            await db.SaveChangesAsync();

            return Ok(products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductsExists(int id)
        {
            return db.Products.Count(e => e.id == id) > 0;
        }
    }
}

