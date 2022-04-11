using Accounts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Business
{
    public class AccountsContext : DbContext
    {

        public AccountsContext(DbContextOptions<AccountsContext> options) : base(options)
        {
        }
        public DbSet<MerchantAccount> Merchants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(
                    @"Server=sovran-accounts.cihpzkqwv66o.eu-west-1.rds.amazonaws.com;
                        Database=sovran_accounts;
                        User=notary;
                        Password=B4rth0l0m3w!;
                        Convert Zero Datetime=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
