import React, {useState} from "react";
import {InputText} from "primereact/inputtext";
import {InputSwitch} from "primereact/inputswitch";

export const MyInput=(props)=>{

    const [val,setVal]= useState();
    return <>
        <label>{props.title}</label>
        <InputText value={val} onChange={(e) => {
            setVal( e.target.value);
            props.parent.state[props.name]=e.target.value;
            props.parent.setState({tmp:Math.random()});
        }} />
    </>
}

export const MySwitcher=(props)=>{

    const [val,setVal]= useState();
    return <>
        <label>{props.title}</label>
        
        <InputSwitch checked={val}
                     onChange={(e) => {
                         setVal( e.target.value);
                         props.parent.state[props.name]=e.target.value;
                         props.parent.setState({tmp:Math.random()});
                     }} />

                    
        {val && <>{props.children}</>}
    </>
}