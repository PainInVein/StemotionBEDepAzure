using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IModelStateCheck
    {
        ResponseDTO<T> CheckModelState<T>(ModelStateDictionary modelState);
    }
}
