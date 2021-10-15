import { useState } from "react";
import { Product } from "./ProductListComponent";

const AddProductCopmonent = (props: any) => {
    const [name, setName] = useState("");
    const [price, setPrice] = useState("");
    const [type, setType] = useState("");

    const handleAddClick = (event: any) => {
        event.preventDefault();
        if (validate()) {
            props.updateProducts(new Product(name, type, parseInt(price), true, 0));
        }
        else {
            alert('Name and type cannot be empty, quantity must be a positive number')
        }
    };

    const validate = (): boolean => {
        if (parseInt(price) > 0 && name && name.length !== 0 && type && type.length !== 0) {
            return true;
        }
        return false;
    }

    return (
        <div className="row">
            <div className="col-md-3 mb-3">
                Name: <input type="text" className="form-control" placeholder="Name" value={name} onChange={e => setName(e.target.value)} required />
            </div>
            <div className="col-md-3 mb-3">
                Price : <input type="number" className="form-control" placeholder="0" value={price} onChange={e => setPrice(e.target.value)} required />
            </div>
            <div className="col-md-3 mb-3">
                Type : <input type="text" className="form-control" placeholder="Type" value={type} onChange={e => setType(e.target.value)} required />
            </div>
            <div className="col-md-3 mb-3">
                <button className="btn btn-success" style={{ float: "right" }} onClick={handleAddClick}>Add</button>
            </div>
        </div>
    );
}

export default AddProductCopmonent