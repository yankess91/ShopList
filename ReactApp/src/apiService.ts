import { addProductToShoppingList, createShoppingList, deleteProductFromShoppingList, getShoppingList } from "./ApiClient";
import { AddProductRequest, Product } from "./Product/ProductListComponent";
import { CreateShoppingListRequest, ShoppingList } from "./ShoppingList/ShoppingListComponent";

export const saveShoppingList = (shoppingListName: string, items: ShoppingList[], setItems: any) => {
    createShoppingList(new CreateShoppingListRequest(shoppingListName))
        .then((result) => {
            if (result.isSuccess) {
                setItems([new ShoppingList(
                    result.shoppingList.id,
                    result.shoppingList.name,
                    [] as Product[])
                    , ...items]);
            }
            else {
                alert(result.ErrorMessage)
            }
        },
            (error) => {
                console.log(error);
            }
        )
}

export const fetchShoppingLists = (setIsLoaded: any, setItems: any, setError: any) => {
    getShoppingList()
        .then(
            (result) => {
                setIsLoaded(true);
                if (result && result.shoppingLists) {
                    let fetchedData = result.shoppingLists.map((sl: any) =>
                        new ShoppingList(sl.id, sl.name, [...sl.products.map((p: any) =>
                            new Product(p.name, p.type, p.price, false, p.id))]));
                    setItems([]);
                    setItems([...fetchedData]);
                }
            },
            (error) => {
                setIsLoaded(true);
                setError(error);
            }
        )
}

export const deleteFromDatabase = (itemsToDelete: number[], products: Product[], setProducts: any, setItemsToDelete: any) => {
    deleteProductFromShoppingList(itemsToDelete)
        .then((result) => {
            if (result.isSuccess) {
                let deletedProductsId: number[] = result.productIds.map((p: number) => p);
                setProducts(products.filter(p => !deletedProductsId.some(dId => p.Id === dId)));
                alert(`${deletedProductsId.length} items were permamently deleted`)
                setItemsToDelete([] as number[]);
            }
            else {
                alert(result.ErrorMessage)
            }
        },
            (error) => {
                console.debug(error);
            }
        )
}

export const addProductsToDatabase = (shoppingListId: number, setProducts: any, setItemsToAdd: any, itemsToAdd: Product[], products: Product[]) => {
    addProductToShoppingList(new AddProductRequest(shoppingListId, itemsToAdd))
        .then((result) => {
            if (result.isSuccess) {
                let addedProducts: Product[] = result.products.map((p: any) => new Product(p.name, p.type, p.price, false, p.id));
                setProducts([...(products.filter(p => !addedProducts.some(ap => ap.Name == p.Name))), ...addedProducts]);
                setItemsToAdd([] as Product[]);
            }
            else {
                alert(result.ErrorMessage)
            }
        },
            (error) => {
                console.log(error);
            }
        )
}