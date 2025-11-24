export interface InventoryImage {
  imageId: number;
  imageUrl: string;
  displayOrder: number;
}

export interface InventoryItem {
  itemId: number;
  name: string;
  description: string;
  price: number;
  primaryPhotoUrl: string;
  category: string;
  isSold: boolean;
  images: InventoryImage[];
}

