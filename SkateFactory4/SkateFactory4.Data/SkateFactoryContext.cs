using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SkateFactory4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkateFactory4.Data;

public class SkateFactoryContext: IdentityDbContext
{
	public SkateFactoryContext(DbContextOptions options) : base(options)
	{

	}

	public DbSet<Skateboard> Skateboards { get; set; }
}
