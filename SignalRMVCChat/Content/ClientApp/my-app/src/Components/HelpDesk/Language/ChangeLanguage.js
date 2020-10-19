import React, {Component} from 'react';
import DefinedLanguages from "./DefinedLanguages";
import Col from "react-bootstrap/Col";

class ChangeLanguage extends Component {
    render() {
        return (
            <div>

                <DefinedLanguages/>
            </div>
        );
    }
}

export default ChangeLanguage;