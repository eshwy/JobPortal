using JopPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JopPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticleAndTitleController : ControllerBase
    {
        private readonly JobPortal2Context _context;
        public ArticleAndTitleController(JobPortal2Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetArticle()
        {
            var ArticleDetails = await _context.ArticleTbls.ToListAsync();
            var PersonalDetails = await _context.PersonalDetailsTbl.ToListAsync();
            List<int> ArticleRowId = new List<int>();
            foreach (var Artdetails in ArticleDetails)
            {
                ArticleRowId.Add(Artdetails.RowId);
            }
            List<ArticleTitleTbl> data = new List<ArticleTitleTbl>();
            foreach (var Idvalue in ArticleRowId)
            {
                var ArticleTitleData = await _context.ArticleTitleTbl.Where(x => x.ArticleId == Idvalue).ToListAsync();
                foreach (var item1 in ArticleTitleData)
                {
                    data.Add(item1);
                }
            }
            var FinalTitleData = from t1 in ArticleDetails
                                 join t2 in data
                                 on t1.RowId equals t2.ArticleId
                                 group t2.Title by t1.RowId into g
                                 select new
                                 {
                                     id = g.Key,
                                     Title = string.Join(",", g.ToArray())
                                 };

            var ArticleData = (from t1 in ArticleDetails
                                    join t2 in FinalTitleData
                                    on t1.RowId equals t2.id
                                    select new
                                    {
                                        t1.RowId,
                                        t1.UserId,
                                        t2.Title,
                                        t1.Category,
                                        t1.Content
                                    }).ToList();

            var FinalArticleData = (from t1 in ArticleData
                                    join t2 in PersonalDetails
                                    on t1.UserId equals t2.UserId 
                                    select new
                                    {
                                        t1.RowId,
                                        t1.UserId,
                                        Name = t2.FirstName +" "+ t2.LastName,
                                        t2.Skills,
                                        t2.Email,
                                        t1.Title,
                                        t1.Category,
                                        t1.Content
                                    });
            return Ok(FinalArticleData);

        }

        [HttpGet("{userId}")]        
        public async Task<ActionResult> GetArticle(int userId)
        {            
            var ArticleDetails = await _context.ArticleTbls.Where(x => x.UserId == userId || x.RowId==userId).ToListAsync();
            List<int> ArticleRowId = new List<int>();
            foreach (var Artdetails in ArticleDetails)
            {
                ArticleRowId.Add(Artdetails.RowId);
            }
            Console.WriteLine(ArticleDetails);
            Console.WriteLine(ArticleRowId);
            List<ArticleTitleTbl> data = new List<ArticleTitleTbl>();
            foreach (var Idvalue in ArticleRowId)
            {
                var ArticleTitleData = await _context.ArticleTitleTbl.Where(x => x.ArticleId == Idvalue).ToListAsync();
                foreach (var item1 in ArticleTitleData)
                {
                    data.Add(item1);
                }
                
            }
            var FinalTitleData = from t1 in ArticleDetails
                                   join t2 in data 
                                   on t1.RowId equals t2.ArticleId
                                   group t2.Title by t1.RowId into g
                                   select new
                                   {                                       
                                       id = g.Key,
                                       Title = string.Join(",",g.ToArray())
                                   };

            var FinalArticleData = (from t1 in ArticleDetails
                                   join t2 in FinalTitleData
                                   on t1.RowId equals t2.id
                                   select new
                                   {
                                       t1.RowId,
                                       t1.UserId,
                                       t2.Title,
                                       t1.Category,
                                       t1.Content
                                   }).ToList();      

            return Ok(FinalArticleData);            
        }

        [HttpPost("ArticlePost")]
        [System.Web.Mvc.ValidateInput(false)]
        public async Task<ActionResult<ArticleTbl>> PostArticle(ArticleTbl article)
        {
            _context.ArticleTbls.Add(article);
            await _context.SaveChangesAsync();

            var data = CreatedAtAction("GetArticle", new { id = article.RowId }, article);
            var d = data.Value;
            var list = new List<ArticleTbl>();

            list.Add((ArticleTbl)d);
            var value = list.Select(x => x.RowId).FirstOrDefault();
            int RowId = value;
            return Ok(RowId);
        }

        [HttpPost]
        [Route("TitlePost")]
        [System.Web.Mvc.ValidateInput(false)]
        public async Task<ActionResult<ArticleTbl>> PostArticle(ArticleTitleTbl articleTitle)
        {
            _context.ArticleTitleTbl.Add(articleTitle);
            await _context.SaveChangesAsync();            
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllUserDetails(int id)
        {
            var Details = await _context.ArticleTitleTbl.Where(c => c.ArticleId == id).ToListAsync();
            if (Details == null)
            {
                return NotFound();
            }
            foreach (var detail in Details)
            {
                _context.ArticleTitleTbl.Remove(detail);
                await _context.SaveChangesAsync();
            }
            var ArticleDetails = await _context.ArticleTbls.FindAsync(id);
            if (ArticleDetails == null)
            {
                return NotFound();
            }
            _context.ArticleTbls.Remove(ArticleDetails);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
