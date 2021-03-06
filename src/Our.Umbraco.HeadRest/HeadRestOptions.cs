﻿using System;
using Our.Umbraco.HeadRest.Interfaces;
using Our.Umbraco.HeadRest.Web.Controllers;
using Our.Umbraco.HeadRest.Web.Mapping;

namespace Our.Umbraco.HeadRest
{
    public class HeadRestOptions : IHeadRestOptions
    {
        public Type ControllerType { get; set; }
        public Func<HeadRestMappingContext, object> Mapper { get; set; }
        public HeadRestViewModelMap ViewModelMappings { get; set; }
        public string RoutesListPath { get; set; }

        public HeadRestOptions()
        {
            ControllerType = typeof(HeadRestController);
            Mapper = (ctx) => AutoMapper.Mapper.Map(ctx.Content, ctx.ContentType, ctx.ViewModelType);
            RoutesListPath = "routes";
        }
    }
}
