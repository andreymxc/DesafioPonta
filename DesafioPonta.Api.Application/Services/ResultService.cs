﻿using FluentValidation.Results;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPonta.Api.Application.Services
{
    public class ResultService
    {
        private const int _badRequest = 400;
        private const int _ok = 200;
        private const int _notFound = 400;
        private const int _internalServerError = 500;
        private const int _forbidden = 401;

        public bool IsSuccess { get; set;}
        public string Message { get; set; }
        public int StatusCode { get; set;}
        public ICollection<ErrorValidation> Errors { get; set; }

        public static ResultService RequestError(string message, ValidationResult validationResult)
        {
            return new ResultService
            {
                IsSuccess = false,
                Message = message,
                StatusCode = 400,
                Errors = validationResult.Errors.Select(i => new ErrorValidation { Field = i.PropertyName, Message = i.ErrorMessage }).ToList()
            };
        }

        public static ResultService<T> RequestError<T>(string message, ValidationResult validationResult)
        {
            return new ResultService<T>
            {
                IsSuccess = false,
                Message = message,
                StatusCode = 400,
                Errors = validationResult.Errors.Select(i => new ErrorValidation { Field = i.PropertyName, Message = i.ErrorMessage }).ToList()
            };
        }

        public static ResultService Fail(string message) => new ResultService() { IsSuccess = false, Message = message, StatusCode = _badRequest };
        public static ResultService<T> Fail<T>(string message) => new ResultService<T> { IsSuccess = false, Message = message, StatusCode = _badRequest };
        public static ResultService Ok(string message) => new ResultService() { IsSuccess = true, Message = message, StatusCode = _ok };
        public static ResultService<T> Ok<T>(T data) => new ResultService<T>() { IsSuccess = true, Data = data, StatusCode = _ok };
        public static ResultService NotFound(string message) => new ResultService() { IsSuccess = false, Message = message, StatusCode = _notFound };
        public static ResultService<T> NotFound<T>(string message) => new ResultService<T> { IsSuccess = false, Message = message, StatusCode = _notFound };
        public static ResultService InternalServerError(string message) => new ResultService() { IsSuccess = false, Message = message, StatusCode = _internalServerError };
        public static ResultService<T> InternalServerError<T>(string message) => new ResultService<T> { IsSuccess = false, Message = message, StatusCode = _internalServerError };
        public static ResultService Forbidden(string message) => new ResultService() { IsSuccess = false, Message = message, StatusCode = _forbidden };
        public static ResultService<T> Forbidden<T>(string message) => new ResultService<T> { IsSuccess = false, Message = message, StatusCode = _forbidden };
    }

    public class ResultService<T> : ResultService
    {
        public T Data { get; set; }
    }
}