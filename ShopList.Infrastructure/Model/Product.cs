using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopList.Infrastructure.Model
{
    public class Product : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Type { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Only positive number allowed.")]
        public int Price { get; set; }

        public int ShoppingListId { get; set; }

        public ShoppingList ShoppingList { get; set; }
    }
}