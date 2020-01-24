using System;
namespace CheeseMVC.Models
{

    /* CheeseMenu object represents a join table of the two objects
     * Cheese and Menu to create a many-to-many relationship
     *  -- the MenuID and CheeseID act as our keys to access the
     *  objects in other tables
     */

    public class CheeseMenu
    {
        public int MenuID { get; set; }
        public Menu Menu { get; set; }

        public int CheeseID { get; set; }
        public Cheese Cheese { get; set; }
    }
}