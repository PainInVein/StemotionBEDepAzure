using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace STEMotion.Application.Helpers
{
    public class ModelStateCheck : IModelStateCheck
    {
        public ResponseDTO<T> CheckModelState<T>(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                var errors = modelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var errorMessage = string.Join("; ", errors);

                return new ResponseDTO<T>(errorMessage, false, default);
            }

            return new ResponseDTO<T>("Model is valid", true, default);
        }
    }
}
