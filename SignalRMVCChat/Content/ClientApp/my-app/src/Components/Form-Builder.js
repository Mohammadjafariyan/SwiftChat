// @flow
import * as React from 'react';
import {useState} from "react";


/*
*   <FormBuilder
              defaultValues={this.state.values}
              form={this.state.form}
              onSubmit={this.onSubmit}
              submitButton={{
                text: "ثبت تنظیمات ربات",
              }}
              defaultInputClass={" form-control "}
              defaultSubmitClass={" btn btn-info mt-5 "}
            />*/
export function FormBuilder(props) {


    console.log('Form-Builder.js-20 - ' + props.defaultValues, props.defaultValues);

    if (props.defaultValues)
        for (let i = 0; i < props.form.length; i++) {
            let item =props.form[i];
            props.form[i][item.name] = props.defaultValues[item.name]
        }
    const [formFields, setFormFields] = useState(props.form);

    const addFormField = () => {
        setFormFields([...formFields, {label: '', value: ''}]);
    };

    const handleFieldChange = (index, field, value) => {
        const updatedFields = [...formFields];
        updatedFields[index] = {...updatedFields[index], [field]: value};
        setFormFields(updatedFields);

    };

    const handleSubmit = (e) => {
        debugger
        e.preventDefault();
        console.log(formFields);

        let form = props.defaultValues;
        for (let i = 0; i < formFields.length; i++) {
            form[formFields[i].name] = formFields[i][formFields[i].name]
        }
        console.log(form);

        props.onSubmit(form);
        // Perform further actions with the form fields
    };

    return (
        <div>
            <form onSubmit={handleSubmit}>
                {formFields.map((field, index) => (
                    <div key={index}>


                        <label>
                            {field.label}
                            <input
                                className={field.defaultInputClass}
                                placeholder={field.placeholder}
                                type="text"
                                value={field[field.name]}
                                onChange={(e) => handleFieldChange(index, field.name, e.target.value)}
                            />
                        </label>
                    </div>
                ))}
                <button type="submit" className={'btn btn-primary'}>
                    {props.submitButton.text}
                </button>
            </form>
        </div>
    );
}

    
