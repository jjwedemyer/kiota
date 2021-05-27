using System.Linq;
using Xunit;

namespace Kiota.Builder.Refiners.Tests {
    public class TypeScriptLanguageRefinerTests {
        private readonly CodeNamespace root = CodeNamespace.InitRootNamespace();
        private const string httpCoreDefaultName = "IHttpCore";
        private const string factoryDefaultName = "ISerializationWriterFactory";
        private const string deserializeDefaultName = "IDictionary<string, Action<Model, IParseNode>>";
        private const string dateTimeOffsetDefaultName = "DateTimeOffset";
        private const string addiationalDataDefaultName = "new Dictionary<string, object>()";
        private const string handlerDefaultName = "IResponseHandler";
        [Fact]
        public void CorrectsCoreType() {

            var model = root.AddClass(new CodeClass (root) {
                Name = "model",
                ClassKind = CodeClassKind.Model
            }).First();
            model.AddProperty(new (model) {
                Name = "core",
                Type = new CodeType(model) {
                    Name = httpCoreDefaultName
                }
            }, new (model) {
                Name = "serializerFactory",
                Type = new CodeType(model) {
                    Name = factoryDefaultName,
                }
            }, new (model) {
                Name = "deserializeFields",
                Type = new CodeType(model) {
                    Name = deserializeDefaultName,
                }
            }, new (model) {
                Name = "someDate",
                Type = new CodeType(model) {
                    Name = dateTimeOffsetDefaultName,
                }
            }, new (model) {
                Name = "additionalData",
                PropertyKind = CodePropertyKind.AdditionalData,
                Type = new CodeType(model) {
                    Name = addiationalDataDefaultName
                }
            });
            var executorMethod = model.AddMethod(new CodeMethod(model) {
                Name = "executor",
                MethodKind = CodeMethodKind.RequestExecutor,
                ReturnType = new CodeType(model) {
                    Name = "string"
                }
            }).First();
            executorMethod.AddParameter(new CodeParameter(executorMethod) {
                Name = "handler",
                Type = new CodeType(executorMethod) {
                    Name = handlerDefaultName,
                }
            });
            const string serializerDefaultName = "ISerializationWriter";
            var serializationMethod = model.AddMethod(new CodeMethod(model) {
                Name = "seriailization",
                MethodKind = CodeMethodKind.Serializer,
                ReturnType = new CodeType(model) {
                    Name = "string"
                }
            }).First();
            serializationMethod.AddParameter(new CodeParameter(serializationMethod) {
                Name = "handler",
                Type = new CodeType(executorMethod) {
                    Name = serializerDefaultName,
                }
            });
            var responseHandlerMethod = model.AddMethod(new CodeMethod(model) {
                Name = "defaultResponseHandler",
                ReturnType = new CodeType(model) {
                    Name = "string"
                }
            }).First();
            const string streamDefaultName = "Stream";
            responseHandlerMethod.AddParameter(new CodeParameter(responseHandlerMethod) {
                Name = "param1",
                Type = new CodeType(responseHandlerMethod) {
                    Name = streamDefaultName
                }
            });
            ILanguageRefiner.Refine(GenerationLanguage.TypeScript, root);
            Assert.Empty(model.GetChildElements(true).OfType<CodeProperty>().Where(x => httpCoreDefaultName.Equals(x.Type.Name)));
            Assert.Empty(model.GetChildElements(true).OfType<CodeProperty>().Where(x => factoryDefaultName.Equals(x.Type.Name)));
            Assert.Empty(model.GetChildElements(true).OfType<CodeProperty>().Where(x => deserializeDefaultName.Equals(x.Type.Name)));
            Assert.Empty(model.GetChildElements(true).OfType<CodeProperty>().Where(x => dateTimeOffsetDefaultName.Equals(x.Type.Name)));
            Assert.Empty(model.GetChildElements(true).OfType<CodeProperty>().Where(x => addiationalDataDefaultName.Equals(x.Type.Name)));
            Assert.Empty(model.GetChildElements(true).OfType<CodeMethod>().SelectMany(x => x.Parameters).Where(x => handlerDefaultName.Equals(x.Type.Name)));
            Assert.Empty(model.GetChildElements(true).OfType<CodeMethod>().SelectMany(x => x.Parameters).Where(x => serializerDefaultName.Equals(x.Type.Name)));
            Assert.Empty(model.GetChildElements(true).OfType<CodeMethod>().SelectMany(x => x.Parameters).Where(x => streamDefaultName.Equals(x.Type.Name)));
        }
    }
}