﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MobilePhoneStoreDBMS.Models.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MobilePhoneStoreDBMSEntities : DbContext
    {
        public MobilePhoneStoreDBMSEntities()
            : base("name=MobilePhoneStoreDBMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AvatarOfProduct> AvatarOfProducts { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SpecificationValue> SpecificationValues { get; set; }
        public virtual DbSet<Specifications_of_all_product> Specifications_of_all_product { get; set; }
        public virtual DbSet<view_Category_List> view_Category_List { get; set; }
    
        [DbFunction("MobilePhoneStoreDBMSEntities", "GetSpecifications")]
        public virtual IQueryable<GetSpecifications_Result> GetSpecifications(Nullable<int> productID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("productID", productID) :
                new ObjectParameter("productID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<GetSpecifications_Result>("[MobilePhoneStoreDBMSEntities].[GetSpecifications](@productID)", productIDParameter);
        }
    
        public virtual int AddNewSpecificationToAProduct(Nullable<int> productID, Nullable<int> specificationID, string value)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            var specificationIDParameter = specificationID.HasValue ?
                new ObjectParameter("SpecificationID", specificationID) :
                new ObjectParameter("SpecificationID", typeof(int));
    
            var valueParameter = value != null ?
                new ObjectParameter("Value", value) :
                new ObjectParameter("Value", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddNewSpecificationToAProduct", productIDParameter, specificationIDParameter, valueParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> sp_Account_Login(string username, string password)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("sp_Account_Login", usernameParameter, passwordParameter);
        }
    
        public virtual ObjectResult<Nullable<bool>> sp_Account_Register(string name, string phoneNumber, string email, string username, string password)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var phoneNumberParameter = phoneNumber != null ?
                new ObjectParameter("PhoneNumber", phoneNumber) :
                new ObjectParameter("PhoneNumber", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("sp_Account_Register", nameParameter, phoneNumberParameter, emailParameter, usernameParameter, passwordParameter);
        }
    
        public virtual ObjectResult<Sp_Catagory_Details_Result> Sp_Catagory_Details(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_Catagory_Details_Result>("Sp_Catagory_Details", idParameter);
        }
    
        public virtual ObjectResult<Sp_Producer_Details_Result> Sp_Producer_Details(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_Producer_Details_Result>("Sp_Producer_Details", idParameter);
        }
    
        public virtual ObjectResult<Sp_Producer_List_Result> Sp_Producer_List()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_Producer_List_Result>("Sp_Producer_List");
        }
    
        public virtual ObjectResult<Sp_Product_Details_Result> Sp_Product_Details(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_Product_Details_Result>("Sp_Product_Details", idParameter);
        }
    
        public virtual ObjectResult<Sp_Product_List_Result> Sp_Product_List()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_Product_List_Result>("Sp_Product_List");
        }
    
        public virtual ObjectResult<Sp_Product_List_Of_Category_Result> Sp_Product_List_Of_Category(Nullable<int> categoryID)
        {
            var categoryIDParameter = categoryID.HasValue ?
                new ObjectParameter("categoryID", categoryID) :
                new ObjectParameter("categoryID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_Product_List_Of_Category_Result>("Sp_Product_List_Of_Category", categoryIDParameter);
        }
    
        public virtual ObjectResult<Sp_Product_List_Of_Producer_Result> Sp_Product_List_Of_Producer(Nullable<int> producerID)
        {
            var producerIDParameter = producerID.HasValue ?
                new ObjectParameter("producerID", producerID) :
                new ObjectParameter("producerID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_Product_List_Of_Producer_Result>("Sp_Product_List_Of_Producer", producerIDParameter);
        }
    }
}
