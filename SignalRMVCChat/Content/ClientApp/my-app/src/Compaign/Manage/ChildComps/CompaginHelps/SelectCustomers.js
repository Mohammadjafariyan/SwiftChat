import React, {Component} from 'react';
import {ListBox} from "primereact/listbox";
import {_GetSelectedCompaign} from "../../CompaignSave";
import {MyCaller,CurrentUserInfo} from "../../../../Help/Socket";
import {InputText} from "primereact/inputtext";
import {Spinner} from "react-bootstrap";
import Container from "react-bootstrap/Container";

class SelectCustomers extends Component {
state={
    searchTerm:'',
    arr:[]
}
    constructor(props) {
        super(props);
        CurrentUserInfo.SelectCustomers=this;
    }
    
    componentDidMount() {
        
    this.setState({loading:true})
        MyCaller.Send('SearchCustomers',{
            searchTerm:this.state.searchTerm
        })
    }


    searchCustomersCallback(res){
        this.setState({loading:false})
        
        if (!res || !res.Content) {
            //  CurrentUserInfo.LayoutPage.showError('هیچ اطلاعاتی دریافت نشد');
            return;
        }
        let arr = res.Content.EntityList ? res.Content.EntityList : res.Content;


        this.setState({arr: arr});
    }

    render() {
        return (
            <div>



                {this.state.loading &&
                <Spinner animation="border" role="status">
                    <span className="sr-only">لطفا منتظر بمانید ....</span>
                </Spinner>}
                
                
                <InputText  value={this.state.searchTerm}
                         placeholder={'جستجو در کل داده ها...'}   onChange={(e) => this.setState({searchTerm: e.target.value})} />

                <hr/>
                <ListBox style={{width:'100%'}} value={_GetSelectedCompaign().selectedCustomers}
                         options={this.state.arr} 
                         onChange={(e) => {

                             _GetSelectedCompaign().selectedCustomers=e.value;
                             this.setState({mat:Math.random()});
                         }}
                         multiple filter optionLabel="Name"
                         listStyle={{maxHeight: '250px', width:'100%'}} />


            </div>
        );
    }
}

export default SelectCustomers;