﻿using System;

namespace Kiota.Builder
{
    public enum CodeParameterKind
    {
        Custom,
        QueryParameter,
        Headers,
        ResponseHandler
    }

    public class CodeParameter : CodeTerminal, ICloneable
    {
        public CodeParameter(CodeElement parent): base(parent)
        {
            
        }
        public CodeParameterKind ParameterKind {get;set;}= CodeParameterKind.Custom;
        public CodeTypeBase Type {get;set;}
        public bool Optional {get;set;}= false;

        public object Clone()
        {
            return new CodeParameter(Parent) {
                Optional = Optional,
                ParameterKind = ParameterKind,
                Name = Name.Clone() as string,
                Type = Type.Clone() as CodeTypeBase,
            };
        }
    }
}