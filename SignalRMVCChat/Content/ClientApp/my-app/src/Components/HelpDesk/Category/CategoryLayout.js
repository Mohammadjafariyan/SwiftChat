import React, {Component} from 'react';
import CategoryIndex from "./CategoryIndex";
import CategorySave from "./CategorySave";
import CategoryDelete from "./CategoryDelete";
import {MyCaller} from "../../../Help/Socket";
import {DataHolder} from "../../../Help/DataHolder";
import LanguageHolder from "../Language/LanguageHolder";
import {Spinner} from "react-bootstrap";
import {_showError, _showMsg} from "../../../Pages/LayoutPage";
import Language from "../Language/Language";
import PrimengModal from "../../SendFromHelpDeskModal";
import DefinedLanguages from "../Language/DefinedLanguages";
import Badge from "react-bootstrap/Badge";
import {CurrentUserInfo} from "../../../CurrentUserInfo";

class CategoryLayout extends Component {
    state={};
    constructor(re) {
        super(re);

        CurrentUserInfo.CategoryLayout = this;
    }

    componentDidMount() {
      
    
    }

   

  
    

    render() {
        
        if (this.state.loading){
            return (<>
                {this.state.loading &&
                <Spinner animation="border" role="status">
                    <span className="sr-only">در حال خواندن اطلاعات...</span>
                </Spinner>}
            </>)
        }
       
       
      
        return (
            <div>
              
            
              
                {!this.state.page && <CategoryIndex/>}
                {this.state.page==='Save' && <CategorySave/>}
                {this.state.page==='Delete' && <CategoryDelete/>}
                
                
            </div>
        );
    }
}

export default CategoryLayout;