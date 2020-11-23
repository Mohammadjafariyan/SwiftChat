import React, {Component} from 'react';
import {CurrentUserInfo,MyCaller} from "../../Help/Socket";
import {Dropdown} from "primereact/dropdown";
import {_showError} from "../../Pages/LayoutPage";
import {Button} from "primereact/button";

class BotLogForm extends Component {
    constructor(props) {
        super(props);
        CurrentUserInfo.BotLogForm = this;

        this.state = {
            order: null,
            selectedbot: null,
            selectedCountry: null,
            botList:[]
        };

        this.orderList = [
            {name: 'آخرین', code: 'NY'},
            {name: 'اولین', code: 'RM'},
        ];

        this.onCityChange = this.onCityChange.bind(this);
        this.onSelectedbotChange = this.onSelectedbotChange.bind(this);
        

    }
    
    componentDidMount() {
        
        MyCaller.Send('GetBotList')
    }

    getBotListCallback(res){
        if (!res || !res.Content){
            _showError('getBotListCallback res is null')
        
        return ;
        }
        
        
        this.setState({botList:res.Content})
        
        
    }

    onCityChange(e) {
        this.setState({order: e.value});
    }

    onSelectedbotChange(e) {
        this.setState({selectedbot: e.value});
    }

    render() {
        return (
            <>
                <Dropdown value={this.state.order} options={this.orderList} onChange={this.onCityChange}
                          optionLabel="name" placeholder="مرتب سازی"/>


                <Dropdown value={this.state.selectedbot} options={this.state.botList} onChange={this.onSelectedbotChange}
                          optionLabel="Name" placeholder="محدود سازی به یک ربات"/>

                          
                          
                          <Button onClick={()=>{

                              CurrentUserInfo.BotLogTable.refreshData()
                              
                          }}>
                              
                              اعمال
                          </Button>
            </>
        );
    }
}

export default BotLogForm;