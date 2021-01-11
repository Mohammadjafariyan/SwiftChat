import {MyCaller} from "./../Help/Socket";
import React, {useState} from "react";
import {CurrentUserInfo} from "../CurrentUserInfo";


export function Input(props,validate) {
    const [val, setVal] = useState(props.value);
    const [error, setError] = useState();
   
   
    React.useEffect(() => {
        setVal(props.value);
    }, [props.value])

    pushToInputs(this);


    function Validate(newVal,newValCal)  {
        //debugger;
        if (props.required && !(newValCal ? newVal : val)) {
            setError(`${props.name} نمی تواند خالی باشد `)
            return false;
        } else {
            setError('')
            return true;
        }
    }

    if (!CurrentUserInfo.Inputs) {
        CurrentUserInfo.Inputs = [];
    }

    CurrentUserInfo.Inputs.push(Validate)
   
    
    return <>

        <label style={{float: 'right'}} dir={'rtl'}>{props.name}</label> 
        
        
        
        {props.type=='textarea' &&   <textarea id={props.id} readonly={props.readonly}    placeholder={props.placeholder}
                                            className={'form-control ' + (props.className ? props.className : '')}
                                            name={props.elname}  value={val}
                                            onChange={(e) => {
                                                setVal(e.target.value);
                                                if (props.onChange) {
                                                    props.onChange(e);
                                                }

                                                if (props.parent && props.parentStateName) {

                                                    props.parent.state[props.parentStateName] = e.target.value;

                                                    props.parent.setState({tmp: Math.random()})
                                                }

                                                Validate(e.target.value,true)
                                            }}> </textarea> }
        
        {props.type!=='textarea' &&   <input


            disabled={props.disabled} id={props.id}
            checked={props.checked}  readonly={props.readonly}  placeholder={props.placeholder}
                                             className={'form-control ' + (props.className ? props.className : '')}
                                             type={props.type}
                                             name={props.elname} value={val}
                                             onChange={(e) => {
                                                 setVal(e.target.value);
                                                 if (props.onChange) {
                                                     props.onChange(e);
                                                 }

                                                 if (props.parent && props.parentStateName) {

                                                     props.parent.state[props.parentStateName] = e.target.value;

                                                     props.parent.setState({tmp: Math.random()})
                                                 }

                                                 Validate(e.target.value,true)
                                             }}/>}
        

        {error && <small style={{color: 'darkred'}}>{error}</small>}
    </>;

}

function pushToInputs(el) {
    if (!CurrentUserInfo.Inputs) {
        CurrentUserInfo.Inputs = [];
    }
    if (CurrentUserInfo.Inputs.find(f => f === el)) {
        CurrentUserInfo.Inputs.push(el);
    }
}


export function CallValidation(){

    if (!CurrentUserInfo.Inputs) {
        CurrentUserInfo.Inputs = [];
    }
    
    let isFormValid=true;
    
    for (let i = 0; i <CurrentUserInfo.Inputs.length; i++) {
       let isValid= CurrentUserInfo.Inputs[i]();
        
        if (!isValid){
            isFormValid=false;
        }
    }
    
    return isFormValid;
}


