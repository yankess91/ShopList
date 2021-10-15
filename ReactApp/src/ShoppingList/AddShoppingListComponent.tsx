import { useState } from "react";
import { ShoppingList } from "./ShoppingListComponent";

const AddShoppingListComponent = (props: any) => {
    const [name, setName] = useState("");

    const handleAddClick = (event: any) => {
        if (validate()) {
            props.AddShoppingList(new ShoppingList(0, name, []));
        }
        else {
            alert("Name cannot be empty");
        }
    };

    const validate = (): boolean => {
        if (name && name.length !== 0) {
            return true;
        }
        return false;
    }

    return (
        <div className="card border-primary mb-3">
            <div className="card-header">Add new shopping list</div>
            <div className="card-body text-primary">
                <div className="row">
                    <div className="col-md-6 mb-3">
                        <input type="text" className="form-control" placeholder="Name" value={name} onChange={e => setName(e.target.value)} required />
                    </div>
                    <div className="col-md-6 mb-3">
                        <button className="btn btn-success" style={{ float: "right" }} onClick={handleAddClick}>Add</button>
                    </div>
                </div></div>
        </div>

    )
}

export default AddShoppingListComponent