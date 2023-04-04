using SkateFactory4.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkateFactory4.Models;

public class EnumSchema
{
	public EBrakeType BrakeType { get; set; }
	public EColor Color { get; set; }
	public EShapeType ShapeType { get; set; }
	public ESkateType SkateType { get; set; }
	public ESkateboardCriteria SkateboardCriteria { get; set; }
}