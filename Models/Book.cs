//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyBookStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            this.Borrows = new HashSet<Borrow>();
        }
    
        public int BookId { get; set; }
        public string BookName { get; set; }
        public Nullable<int> AuthorId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<System.DateTime> BuyingDate { get; set; }
        public Nullable<int> ReadingId { get; set; }
        public string LendStatus { get; set; }
        public string Photo { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> BookStatusId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string BorrowerName { get; set; }
        public Nullable<System.DateTime> BorrowDate { get; set; }
    
        public virtual Author Author { get; set; }
        public virtual BookStatu BookStatu { get; set; }
        public virtual Category Category { get; set; }
        public virtual Reading Reading { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Borrow> Borrows { get; set; }
    }
}
