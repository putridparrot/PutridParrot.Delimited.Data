using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Delimited.Data
{
	/// <summary>
	/// Allows us to define the field mappings in the app.config
	/// </summary>
	/// <example>
	/// section = (DelimitedSerializerMappingsConfigurationSection) ConfigurationManager.GetSection("DelimitedMappings");
	/// 
	/// in the config file 
	/// 
	/// <section name="DelimitedMappings" type="Namespace, Assembly" allowDefinition="Everywhere" allowExeDefinition="MachineToApplication" restartOnExternalChanges="true" />
	///</example>
	public class DelimitedSerializerMappingsConfigurationSection : ConfigurationSection
	{
		public DelimitedSerializerMappingsElementCollection Mappings
		{
			get { return (DelimitedSerializerMappingsElementCollection) this["DelimitedSerializerMappings"]; }
			set { this["DelimitedSerializerMappings"] = value; } 
		}
	}

	public class DelimitedSerializerMappingsElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new DelimitedSerializerMappingsElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((DelimitedSerializerMappingsElement) element).Property;
		}

		protected override string ElementName
		{
			get { return "Mapping"; }
		}
	}

	public class DelimitedSerializerMappingsElement : ConfigurationElement
	{
		[ConfigurationProperty("Heading", IsRequired = false)]
		public string Heading
		{
			get { return (string) this["Heading"]; }
			set { this["Heading"] = value; }
		}

		[ConfigurationProperty("ColumnIndex", IsRequired = false)]
		public string ColumnIndex
		{
			get { return (string)this["ColumnIndex"]; }
			set { this["ColumnIndex"] = value; }
		}

		[ConfigurationProperty("Property", IsRequired = true)]
		public string Property
		{
			get { return (string)this["Property"]; }
			set { this["Property"] = value; }
		}
	}
}
