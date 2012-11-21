﻿using Nancy;
using Nancy.Conventions;
public class CustomBootstrapper : DefaultNancyBootstrapper
{
    protected override void ConfigureConventions(NancyConventions conventions)
    {
        base.ConfigureConventions(conventions);

        conventions.StaticContentsConventions.Add(
            StaticContentConventionBuilder.AddFile("/index.html", "index.html")
        );
    }
}