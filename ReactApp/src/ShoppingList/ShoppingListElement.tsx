import { ListGroup } from 'react-bootstrap';
import ProductListComponent, { Product } from '../Product/ProductListComponent';
import { ShoppingList } from './ShoppingListComponent';

export class ShoppingListElemenProps{
    ShoppingList: ShoppingList;
    DeleteShoppingList: any;

    constructor(shoppingList: ShoppingList){
        this.ShoppingList = shoppingList;
    }
}

const ShoppingListElement = (props: ShoppingListElemenProps) =>{
    return(
        <ListGroup className="my-3">
            <ListGroup.Item key={props.ShoppingList.Id} variant="primary">
                <h3>Product list: {props.ShoppingList.Name}</h3>
                <h4>Products:</h4>
                <ProductListComponent
                    Products={props.ShoppingList.Products}
                    ShoppingListId={props.ShoppingList.Id}
                    DeleteShoppingList={(shoppingListId: number) => {props.DeleteShoppingList(shoppingListId)}}
                />
            </ListGroup.Item>
        </ListGroup>
    )

}

export default ShoppingListElement