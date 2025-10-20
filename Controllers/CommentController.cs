using Microsoft.AspNetCore.Mvc;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Exceptions;
using ShopPC.Models;
using ShopPC.Service.ImplementationsService;
using ShopPC.Service.InterfaceService;
using System.ComponentModel.DataAnnotations;

namespace ShopPC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("create/{accountId}/{productId}")]
        public async Task<ActionResult<ApiResponse<CommentResponse>>> createComment(
            [FromRoute(Name = "productId")] string productId,
            [FromRoute(Name = "accountId")] string accountId,
            [FromForm(Name = "content")][Required] string content,
            [FromForm(Name ="rating")][Required] int rating)
        {
            var request = new CommentRequest
            {
                content = content,
                rating = rating
            };
            var response = new ApiResponse<CommentResponse>()
            {
                Message = "Create comment Successfully"
            };

            try
            {
                var comment = await _commentService.CreateComment(accountId, productId, request);
                response.Result = comment;
                return Ok(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return BadRequest(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return StatusCode(500, response);
            }
        }

        [HttpPut("update/{commentId}")]
        public async Task<ActionResult<ApiResponse<CommentResponse>>> updateComment(
            [FromRoute(Name = "commentId")] string commentId,
            [FromForm(Name = "content")] string? content,
            [FromForm(Name ="rating")] int? rating)
        {
            var request = new CommentRequest
            {
                content = content?? string.Empty,
                rating = rating ?? 0
            };
            var response = new ApiResponse<CommentResponse>()
            {
                Message = "Update comment Successfully"
            };
            try
            {
                var comment = await _commentService.UpdateComment(commentId, request);
                response.Result = comment;
                return Ok(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return BadRequest(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return StatusCode(500, response);
            }
        }
        [HttpDelete("delete/{commentId}")]
        public async Task<ActionResult<ApiResponse<string>>> deleteComment(
            [FromRoute(Name = "commentId")] string commentId)
        {
            var response = new ApiResponse<string>()
            {
                Message = "Delete comment Successfully"
            };
            try
            {
                var result = await _commentService.DeleteComment(commentId);
                response.Result = result;
                return Ok(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return BadRequest(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return StatusCode(500, response);
            }
        }


        [HttpGet("getByProduct/{productId}")]
        public async Task<ActionResult<ApiResponse<List<CommentResponse>>>> getCommentsByProductId(
            [FromRoute(Name = "productId")] string productId)
        {
            var response = new ApiResponse<List<CommentResponse>>()
            {
                Message = "Get comments by productId Successfully"
            };
            try
            {
                var comments = await _commentService.GetCommentsByProductId(productId);
                response.Result = comments;
                return Ok(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return BadRequest(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return StatusCode(500, response);
            }
        }

        [HttpGet("getByAccount/{accountId}")]
        public async Task<ActionResult<ApiResponse<List<CommentResponse>>>> getCommentsByAccountId(
            [FromRoute(Name = "accountId")] string accountId)
        {
            var response = new ApiResponse<List<CommentResponse>>()
            {
                Message = "Get comments by accountId Successfully"
            };
            try
            {
                var comments = await _commentService.GetCommentsByAccountId(accountId);
                response.Result = comments;
                return Ok(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return BadRequest(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return StatusCode(500, response);
            }
        }

        [HttpGet("summary/{productId}")]
        public async Task<ActionResult<ApiResponse<RatingSummaryResponse>>> getRatingSummary(
            [FromRoute(Name ="productId")] string productId)
        {
            var resp = new ApiResponse<RatingSummaryResponse>();
            try
            {
                resp.Result = await _commentService.GetRatingSummaryByProductId(productId);
                resp.Message = "Get rating summary successfully";
                return Ok(resp);
            }
            catch (AppException ex)
            {
                resp.Code = 400; resp.Message = ex.Message;
                return BadRequest(resp);
            }
            catch (Exception ex)
            {
                resp.Code = 500; resp.Message = ex.Message;
                return StatusCode(500, resp);
            }
        }
    }
}
