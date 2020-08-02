
namespace PixelHunter1995.InventoryLib
{
    class InventoryManager
    {
        public Inventory inventory { get; internal set; }

        public InventoryManager()
        {
            inventory = new Inventory();
        }
        public void Initialize(string tilesetDirPath)
        {
            inventory.Initialize(tilesetDirPath);
        }
    }
}
