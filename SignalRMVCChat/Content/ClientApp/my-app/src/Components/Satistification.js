import React, {Component} from 'react';
import {MyCaller,CurrentUserInfo} from "../Help/Socket";
import {_showError} from "../Pages/LayoutPage";
import {DataHolder} from "../Help/DataHolder";

class Satistification extends Component {
    state={ratingCount:0}

    constructor(props) {
        super(props);
        CurrentUserInfo.Satistification=this;
    }


    componentDidMount() {
        if (!DataHolder.selectedCustomer){
            return ;
        }

        MyCaller.Send("GetRating",{customerId:DataHolder.selectedCustomer.Id,id:DataHolder.selectedCustomer.Id})
    }

    getRatingCallback(res){
        if (!res || !res.Content) {
            _showError("دیتای بازگشتی از سرور نال است")
            return;
        }

        if (!DataHolder.selectedCustomer){
            return ;
        }

        if (res.Content.CustomerId===DataHolder.selectedCustomer.Id)
        {
            this.setState({ratingCount:res.Content.RatingCount})
        }



    }
    render() {
        if (!DataHolder.selectedCustomer){
            return <></>;
        }

        let stars=[];
        for (let i = 1; i <= 6; i++) {
            stars.push(i);
        }
        
        return (
            <div aria-label="رضایتمندی" data-microtip-position="top" role="tooltip">


                {stars.map((val,i,arr)=>{
                    
                    if (i<this.state.ratingCount){
                        return                <i style={{color:'#f8d007'}} className="fa fa-star" aria-hidden="true"></i>

                    }else{
                        return                 <i style={{color:'black'}} className="fa fa-star" aria-hidden="true"></i>
;
                    }
                })}
                
{/*
                <i style={{color:'#f8d007'}} className="fa fa-star" aria-hidden="true"></i>
                <i style={{color:'#f8d007'}} className="fa fa-star" aria-hidden="true"></i>
                <i style={{color:'#f8d007'}} className="fa fa-star" aria-hidden="true"></i>
              
                <i style={{color:'#f8d007'}} className="fa fa-star" aria-hidden="true"></i>
                <i style={{color:'black'}} className="fa fa-star" aria-hidden="true"></i>
                <i style={{color:'black'}} className="fa fa-star" aria-hidden="true"></i>*/}

            </div>
        );
    }
}

export default Satistification;