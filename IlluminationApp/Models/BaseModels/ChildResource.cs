using System.Xml.Serialization;

namespace IlluminationApp.BaseModels
{
	/// <summary>
	/// Abstract class for resources that have a parent resource. <see cref="TParent"/> is the generic type of the parent resource.
	/// </summary>
	/// <typeparam name="TParent"></typeparam>
	public abstract class ChildResource<TParent> : Resource where TParent : class, new()
	{
		[XmlElement("Parent")]
		public int Parent { get; set; }

		public ChildResource() : base()
		{
		}

		public ChildResource(string name) : base(name)
		{
		}
	}
}
