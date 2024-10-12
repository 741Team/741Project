//-----------------------------------------------------------------------------
// <auto-generated>
//     This file was generated by the C# SDK Code Generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//-----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.Services.Core.Environments.Client.Http;



namespace Unity.Services.Core.Environments.Client.Models
{
    /// <summary>
    /// UnityCreateProjectRequest model
    /// </summary>
    [Preserve]
    [DataContract(Name = "unity_createProject_request")]
    internal class UnityCreateProjectRequest
    {
        /// <summary>
        /// Creates an instance of UnityCreateProjectRequest.
        /// </summary>
        /// <param name="name">Name of the project</param>
        /// <param name="coppa">COPPA value for the project</param>
        [Preserve]
        public UnityCreateProjectRequest(string name, CoppaOptions coppa)
        {
            Name = name;
            Coppa = coppa;
        }

        /// <summary>
        /// Name of the project
        /// </summary>
        [Preserve]
        [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
        public string Name{ get; }
        
        /// <summary>
        /// COPPA value for the project
        /// </summary>
        [Preserve]
        [JsonConverter(typeof(StringEnumConverter))]
        [DataMember(Name = "coppa", IsRequired = true, EmitDefaultValue = true)]
        public CoppaOptions Coppa{ get; }
    
        /// <summary>
        /// COPPA value for the project
        /// </summary>
        /// <value>COPPA value for the project</value>
        [Preserve]
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CoppaOptions
        {
            /// <summary>
            /// Enum Compliant for value: compliant
            /// </summary>
            [EnumMember(Value = "compliant")]
            Compliant = 1,
            /// <summary>
            /// Enum NotCompliant for value: not_compliant
            /// </summary>
            [EnumMember(Value = "not_compliant")]
            NotCompliant = 2
        }

        /// <summary>
        /// Formats a UnityCreateProjectRequest into a string of key-value pairs for use as a path parameter.
        /// </summary>
        /// <returns>Returns a string representation of the key-value pairs.</returns>
        internal string SerializeAsPathParam()
        {
            var serializedModel = "";

            if (Name != null)
            {
                serializedModel += "name," + Name + ",";
            }
            serializedModel += "coppa," + Coppa;
            return serializedModel;
        }

        /// <summary>
        /// Returns a UnityCreateProjectRequest as a dictionary of key-value pairs for use as a query parameter.
        /// </summary>
        /// <returns>Returns a dictionary of string key-value pairs.</returns>
        internal Dictionary<string, string> GetAsQueryParam()
        {
            var dictionary = new Dictionary<string, string>();

            if (Name != null)
            {
                var nameStringValue = Name.ToString();
                dictionary.Add("name", nameStringValue);
            }
            
            var coppaStringValue = Coppa.ToString();
            dictionary.Add("coppa", coppaStringValue);
            
            return dictionary;
        }
    }
}