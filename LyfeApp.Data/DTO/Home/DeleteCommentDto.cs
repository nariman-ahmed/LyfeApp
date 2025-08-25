using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyfeApp.Data.DTO.Home
{
    public class DeleteCommentDto
    {
        //we could generally just pass the int id to the httpdelete method since its just one parameter (el dto da kan fee comment id bas fel awel)
        //directly from front-end, but lets keep it consistent and have DTOs for all out cases.
        public int CommentId { get; set; }    //COMMENT ID
        public int PostId { get; set; }       //POST ID
    }
}