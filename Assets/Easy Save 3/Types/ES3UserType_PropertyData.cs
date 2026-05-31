using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("m_soul", "m_seed")]
	public class ES3UserType_PropertyData : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_PropertyData() : base(typeof(PropertyData)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (PropertyData)obj;
			
			writer.WritePrivateField("m_soul", instance);
			writer.WritePrivateField("m_seed", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (PropertyData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "m_soul":
					instance = (PropertyData)reader.SetPrivateField("m_soul", reader.Read<System.Int32>(), instance);
					break;
					case "m_seed":
					instance = (PropertyData)reader.SetPrivateField("m_seed", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new PropertyData();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_PropertyDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_PropertyDataArray() : base(typeof(PropertyData[]), ES3UserType_PropertyData.Instance)
		{
			Instance = this;
		}
	}
}