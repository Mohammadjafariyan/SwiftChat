import React, {Component} from 'react';
import ContactsIndex from "./ContactsIndex";

class ContactsLayout extends Component {
    render() {
        return (
            <div>

                
                <h3>لیست کنتاکت ها (کاربران)</h3>
                <ContactsIndex/>

            </div>
        );
    }
}

export default ContactsLayout;