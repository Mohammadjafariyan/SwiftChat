import React, {Component, useState} from 'react';
import { InputGroup, Form, Card, Container } from "react-bootstrap";
import { MyCaller, CurrentUserInfo } from "./../Help/Socket";

import '../styles/myStyle.css';

function addNewTag(tagName) {
    if (!tagName ) {

        CurrentUserInfo.LayoutPage.showError('لطفا ابتدا عنوان برچسب را وارد نمایید');
        return;
    }

    CurrentUserInfo.LayoutPage.showMsg('در حال ثبت برچسب جدید')
    MyCaller.Send("NewTagAdd", {tagTitle: tagName});

}

function AddTag() {
    CurrentUserInfo.AddTag=this;
    
    const [tagName, setTagName] = useState('');

    
        return (
            <div>
                <InputGroup>
                    <InputGroup.Prepend>
                        <InputGroup.Text onClick={()=>{
                            addNewTag(tagName);
                            setTagName('');
                        }} className='pointer' id="inputGroupPrepend">

                            <i className={'fa fa-plus'}></i>

                        </InputGroup.Text>
                    </InputGroup.Prepend>
                    <Form.Control
                        type="text"
                        placeholder="عنوان برچسب را وارد نمایید"
                        aria-describedby="inputGroupPrepend"
                        name="عنوان تگ"
                        value={tagName}
                        onChange={(e) => setTagName(e.target.value)}
                    />
                </InputGroup>
                
                
            </div>
        );
}

export default AddTag;