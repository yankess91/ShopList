import { useEffect, useState } from 'react';
import { Product } from '../Product/ProductListComponent';
import { hubEndpint, hubEventOnProductUpdate, hubEventOnShoppingListtUpdate } from '../ApiClient'
import { fetchShoppingLists, saveShoppingList } from '../apiService'
import AddShoppingListComponent from './AddShoppingListComponent';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import ShoppingListElement from './ShoppingListElement';

export class ShoppingList {
    Id: number;
    Name: string;
    Products: Product[];

    constructor(id: number, name: string, products: Product[]) {
        this.Id = id;
        this.Name = name;
        this.Products = products;
    }
}

export class CreateShoppingListRequest {
    Name: string;

    constructor(name: string) {
        this.Name = name;
    }
}

const ShoppingListComponent = () => {
    let errorInitail: any;

    const [connection, setConnection] = useState<HubConnection>();
    const [error, setError] = useState(errorInitail);
    const [isLoaded, setIsLoaded] = useState(false);
    const [items, setItems] = useState<ShoppingList[]>([] as ShoppingList[]);

    const addShoppingList = (shoppingList: ShoppingList) => {
        if (items.some(p => p.Name === shoppingList.Name)) {
            alert("there is an shopping list with this name on the list")
        }
        else {
            saveShoppingList(shoppingList.Name, items, setItems);
        }
    }

    useEffect(() => {
        fetchShoppingLists(setIsLoaded, setItems, setError);
    }, [])

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(hubEndpint)
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    connection.on(hubEventOnProductUpdate, message => {
                        if (items) {
                            let tempItems = items.filter(item => item.Products.some(p => p.IsNew));
                        }
                        fetchShoppingLists(setIsLoaded, setItems, setError);
                    });
                    connection.on(hubEventOnShoppingListtUpdate, message => {
                        fetchShoppingLists(setIsLoaded, setItems, setError);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection])

    if (error) {
        return <div>Error: {error.message}</div>;
    } else if (!isLoaded) {
        return <div>Loading...</div>;
    } else {
        return (
            <>
                <AddShoppingListComponent
                    AddShoppingList={(shoppingList: ShoppingList) => addShoppingList(shoppingList)}
                />
                {items.map((item) =>
                    <ShoppingListElement
                        ShoppingList={item}
                        DeleteShoppingList={(shoppingListId: number) => { setItems(items.filter(item => !(item.Id === shoppingListId))) }}
                    />
                )}
            </>
        );
    }
}

export default ShoppingListComponent