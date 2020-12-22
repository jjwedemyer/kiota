﻿namespace kiota.core {
    public class GenerationConfiguration {
        public string OpenAPIFilePath { get; set; } = "openapi.yaml";
        public string OutputPath { get; set; } = "./output";
        public string ClientClassName { get; set; } = "GraphClient";
    }
}