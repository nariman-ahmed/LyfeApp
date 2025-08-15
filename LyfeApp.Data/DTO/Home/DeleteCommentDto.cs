using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyfeApp.Data.DTO.Home
{
    public class DeleteCommentDto
    {
        //we could generally just pass the int id to the httpdelete method since its just one parameter
        //directly from front-end, but lets keep it consistent and have DTOs for all out cases.
        public int Id { get; set; }    //COMMENT ID
    }
}