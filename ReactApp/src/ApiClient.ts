export const getShoppingList = async (): Promise<any> => {
    return getData(apiEndpoint + "ShoppingList")
}

export const addProductToShoppingList = async (dto: any): Promise<any> => {
    return postData(apiEndpoint + "api/Product", dto)
}

export const createShoppingList = async (dto: any): Promise<any> => {
    return postData(apiEndpoint + "ShoppingList", dto)
}

export const deleteProductFromShoppingList = async (dto: any): Promise<any> => {
    return deleteData(apiEndpoint + "api/Product", dto)
}

export const deleteShoppingList = async (dto: any): Promise<any> => {
    return deleteData(apiEndpoint + "ShoppingList", dto)
}

export const apiEndpoint: string = "http://localhost:19588/";

export const hubEndpint: string = apiEndpoint + "hubs/shoppinglist";

export const hubEventOnProductUpdate: string = "ProductListUpdate";

export const hubEventOnShoppingListtUpdate: string = "ShoppingListUpdate";

const postData = async (url: string, dto: any): Promise<any> => {
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(dto)
    });
    return response.json();
}

const getData = async (url: string): Promise<any> => {
    const response = await fetch(url);
    return response.json();
}

const deleteData = async (url: string, dto: any): Promise<any> => {
    const response = await fetch(url, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
        , body: JSON.stringify(dto)
    });
    return response.json();
}