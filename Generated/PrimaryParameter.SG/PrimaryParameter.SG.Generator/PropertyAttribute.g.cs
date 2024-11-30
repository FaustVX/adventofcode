﻿#pragma warning disable
// <auto-generated/>
using global::System;
using global::System.Diagnostics;
namespace PrimaryParameter.SG
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = true)]
    [Conditional("DEBUG")]
    sealed class PropertyAttribute : Attribute
    {
        public string Name { get; init; }
        public string AssignFormat { get; init; }
        public Type Type { get; init; }
        public string Setter { get; init; }
        public string Scope { get; init; }
        public string Summary { get; init; }
        public bool WithoutBackingStorage { get; init; }
    }
}
