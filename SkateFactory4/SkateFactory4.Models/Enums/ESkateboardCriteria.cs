using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SkateFactory4.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ESkateboardCriteria
{
    All,
    Electric,
    Regular
}
