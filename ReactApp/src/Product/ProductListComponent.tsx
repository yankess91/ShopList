import React, { useState } from "react"
import { ListGroup } from "react-bootstrap"
import { addProductToShoppingList, deleteProductFromShoppingList, deleteShoppingList } from "../ApiClient";
import { addProductsToDatabase, deleteFromDatabase } from "../apiService";
import AddProductCopmonent from "./AddProductCopmonent"

export class Product {
    Id: number;
    Name: string;
    Type: string;
    Price: number;
    IsNew: boolean;

    constructor(name: string, type: string, price: number, isNew: boolean, id: number) {
        this.Name = name;
        this.Type = type;
        this.Price = price;
        this.IsNew = isNew;
        this.Id = id;
    }
}

export class AddProductRequest {
    ShoppingListId: number;
    Products: Product[];

    constructor(shoppingListId: number, products: Product[]) {
        this.ShoppingListId = shoppingListId;
        this.Products = products;
    }
}

class ProductListProps {
    Products: Product[] = [];
    ShoppingListId: number = 0;
    DeleteShoppingList: any;
}

const ProductListComponent = (props: ProductListProps) => {
    const [products, setProducts] = useState(props.Products);
    const [itemsToAdd, setItemsToAdd] = useState<Product[]>([] as Product[]);
    const [itemsToDelete, setItemsToDelete] = useState<number[]>([] as number[]);

    const updateProductsList = (product: Product) => {
        if (products.some(p => p.Name === product.Name)) {
            alert("there is an item with this name on the list")
        }
        else {
            setProducts([...products, product])
            setItemsToAdd([...itemsToAdd, product])
        }
    }

    const handleDeleteShoppingList = () => {
        deleteShoppingList(props.ShoppingListId)
            .then((result) => {
                if (result.isSuccess) {
                    props.DeleteShoppingList(result.shoppingListId);
                    alert(`Shopping list was permamently deleted`)
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

    const handleDeleteButton = (product: Product) => {
        if (!product.IsNew) {
            setItemsToDelete([...itemsToDelete, product.Id]);
        }
        setItemsToAdd(products.filter(p => p.Name !== product.Name));
        setProducts(products.filter(p => p.Name !== product.Name));
    }

    const handleSaveChanges = async () => {
        if (itemsToDelete.length > 0) {
            deleteFromDatabase(itemsToDelete, products, setProducts, setItemsToDelete)
        }
        if (itemsToAdd.length > 0) {
            addProductsToDatabase(props.ShoppingListId, setProducts, setItemsToAdd, itemsToAdd, products)
        }
        alert("Sync completed")
    };

    return (
        <ListGroup>
            {products.map((product, index) =>
                <ListGroup.Item variant={product.IsNew ? "warning" : ""} className="my-2" key={product.IsNew ? `temp_${index}` : product.Id}>
                    <b>Name :</b> {product.Name}         <b>Price :</b> {product.Price}         <b>Type :</b> {product.Type} 
                    <button className="btn btn-danger" style={{ float: "right" }} onClick={() => handleDeleteButton(product)}>Delete</button>
                </ListGroup.Item>
            )}
            <AddProductCopmonent
                updateProducts={(newProduct: Product) => updateProductsList(newProduct)}
            />
            <button className="btn btn-success" onClick={handleSaveChanges}>Save changes</button>
            <button className="btn btn-danger" onClick={handleDeleteShoppingList}>Delete shopping list</button>
        </ListGroup>
    )
}

export default ProductListComponent