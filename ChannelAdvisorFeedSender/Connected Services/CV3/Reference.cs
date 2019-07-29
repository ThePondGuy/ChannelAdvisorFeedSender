﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChannelAdvisorFeedSender.CV3 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Name="CV3Data.xsdPortType", Namespace="https://service.commercev3.com/CV3Data.xsd", ConfigurationName="CV3.CV3DataxsdPortType")]
    public interface CV3DataxsdPortType {
        
        // CODEGEN: Generating message contract since the wrapper namespace (http://soapinterop.org/) of message testRequest does not match the default value (https://service.commercev3.com/CV3Data.xsd)
        [System.ServiceModel.OperationContractAttribute(Action="https://service.commercev3.com/index.php/test", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        ChannelAdvisorFeedSender.CV3.testResponse test(ChannelAdvisorFeedSender.CV3.testRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://service.commercev3.com/index.php/test", ReplyAction="*")]
        System.Threading.Tasks.Task<ChannelAdvisorFeedSender.CV3.testResponse> testAsync(ChannelAdvisorFeedSender.CV3.testRequest request);
        
        // CODEGEN: Generating message contract since the wrapper namespace (http://soapinterop.org/) of message CV3DataRequest does not match the default value (https://service.commercev3.com/CV3Data.xsd)
        [System.ServiceModel.OperationContractAttribute(Action="https://service.commercev3.com/index.php/CV3Data", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        ChannelAdvisorFeedSender.CV3.CV3DataResponse CV3Data(ChannelAdvisorFeedSender.CV3.CV3DataRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://service.commercev3.com/index.php/CV3Data", ReplyAction="*")]
        System.Threading.Tasks.Task<ChannelAdvisorFeedSender.CV3.CV3DataResponse> CV3DataAsync(ChannelAdvisorFeedSender.CV3.CV3DataRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="test", WrapperNamespace="http://soapinterop.org/", IsWrapped=true)]
    public partial class testRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string testparam1;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=1)]
        public string testparam2;
        
        public testRequest() {
        }
        
        public testRequest(string testparam1, string testparam2) {
            this.testparam1 = testparam1;
            this.testparam2 = testparam2;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="testResponse", WrapperNamespace="http://soapinterop.org/", IsWrapped=true)]
    public partial class testResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        [System.Xml.Serialization.SoapElementAttribute(DataType="base64Binary")]
        public byte[] @return;
        
        public testResponse() {
        }
        
        public testResponse(byte[] @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CV3Data", WrapperNamespace="http://soapinterop.org/", IsWrapped=true)]
    public partial class CV3DataRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string data;
        
        public CV3DataRequest() {
        }
        
        public CV3DataRequest(string data) {
            this.data = data;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CV3DataResponse", WrapperNamespace="http://soapinterop.org/", IsWrapped=true)]
    public partial class CV3DataResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        [System.Xml.Serialization.SoapElementAttribute(DataType="base64Binary")]
        public byte[] @return;
        
        public CV3DataResponse() {
        }
        
        public CV3DataResponse(byte[] @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CV3DataxsdPortTypeChannel : ChannelAdvisorFeedSender.CV3.CV3DataxsdPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CV3DataxsdPortTypeClient : System.ServiceModel.ClientBase<ChannelAdvisorFeedSender.CV3.CV3DataxsdPortType>, ChannelAdvisorFeedSender.CV3.CV3DataxsdPortType {
        
        public CV3DataxsdPortTypeClient() {
        }
        
        public CV3DataxsdPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CV3DataxsdPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CV3DataxsdPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CV3DataxsdPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ChannelAdvisorFeedSender.CV3.testResponse ChannelAdvisorFeedSender.CV3.CV3DataxsdPortType.test(ChannelAdvisorFeedSender.CV3.testRequest request) {
            return base.Channel.test(request);
        }
        
        public byte[] test(string testparam1, string testparam2) {
            ChannelAdvisorFeedSender.CV3.testRequest inValue = new ChannelAdvisorFeedSender.CV3.testRequest();
            inValue.testparam1 = testparam1;
            inValue.testparam2 = testparam2;
            ChannelAdvisorFeedSender.CV3.testResponse retVal = ((ChannelAdvisorFeedSender.CV3.CV3DataxsdPortType)(this)).test(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ChannelAdvisorFeedSender.CV3.testResponse> ChannelAdvisorFeedSender.CV3.CV3DataxsdPortType.testAsync(ChannelAdvisorFeedSender.CV3.testRequest request) {
            return base.Channel.testAsync(request);
        }
        
        public System.Threading.Tasks.Task<ChannelAdvisorFeedSender.CV3.testResponse> testAsync(string testparam1, string testparam2) {
            ChannelAdvisorFeedSender.CV3.testRequest inValue = new ChannelAdvisorFeedSender.CV3.testRequest();
            inValue.testparam1 = testparam1;
            inValue.testparam2 = testparam2;
            return ((ChannelAdvisorFeedSender.CV3.CV3DataxsdPortType)(this)).testAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ChannelAdvisorFeedSender.CV3.CV3DataResponse ChannelAdvisorFeedSender.CV3.CV3DataxsdPortType.CV3Data(ChannelAdvisorFeedSender.CV3.CV3DataRequest request) {
            return base.Channel.CV3Data(request);
        }
        
        public byte[] CV3Data(string data) {
            ChannelAdvisorFeedSender.CV3.CV3DataRequest inValue = new ChannelAdvisorFeedSender.CV3.CV3DataRequest();
            inValue.data = data;
            ChannelAdvisorFeedSender.CV3.CV3DataResponse retVal = ((ChannelAdvisorFeedSender.CV3.CV3DataxsdPortType)(this)).CV3Data(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ChannelAdvisorFeedSender.CV3.CV3DataResponse> ChannelAdvisorFeedSender.CV3.CV3DataxsdPortType.CV3DataAsync(ChannelAdvisorFeedSender.CV3.CV3DataRequest request) {
            return base.Channel.CV3DataAsync(request);
        }
        
        public System.Threading.Tasks.Task<ChannelAdvisorFeedSender.CV3.CV3DataResponse> CV3DataAsync(string data) {
            ChannelAdvisorFeedSender.CV3.CV3DataRequest inValue = new ChannelAdvisorFeedSender.CV3.CV3DataRequest();
            inValue.data = data;
            return ((ChannelAdvisorFeedSender.CV3.CV3DataxsdPortType)(this)).CV3DataAsync(inValue);
        }
    }
}