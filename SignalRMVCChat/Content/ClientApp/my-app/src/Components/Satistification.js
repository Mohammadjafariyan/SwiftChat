import React, {Component} from 'react';
import {MyCaller} from "../Help/Socket";
import {_showError, _showMsg} from "../Pages/LayoutPage";
import {DataHolder} from "../Help/DataHolder";
import {Inplace, InplaceContent, InplaceDisplay} from "primereact/inplace";
import Button from "react-bootstrap/Button";
import {CurrentUserInfo} from "../CurrentUserInfo";

class Satistification extends Component {
    state={ratingCount:[]}

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
            
            if (res.Content.IsNew)
            _showMsg("بازخورد کاربر دریافت شد",'success')
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
            
            <>
                <Inplace closable active={this.state.active} onToggle={(e) => this.setState({active: e.value})}>
                    <InplaceDisplay>
                        <div aria-label="رضایتمندی ، برای ارسال کلیک کنید   " data-microtip-position="top" role="tooltip">


                            {stars.map((val,i,arr)=>{

                                if (i<this.state.ratingCount.length){
                                    return                <i key={i} style={{color:'#f8d007'}} className="fa fa-star" aria-hidden="true"></i>

                                }else{
                                    return                 <i key={i} style={{color:'black'}} className="fa fa-star" aria-hidden="true"></i>
                                        ;
                                }
                            })}


                            <small>:رضایتمندی</small>
                            {/*
                <i style={{color:'#f8d007'}} className="fa fa-star" aria-hidden="true"></i>
                <i style={{color:'#f8d007'}} className="fa fa-star" aria-hidden="true"></i>
                <i style={{color:'#f8d007'}} className="fa fa-star" aria-hidden="true"></i>
              
                <i style={{color:'#f8d007'}} className="fa fa-star" aria-hidden="true"></i>
                <i style={{color:'black'}} className="fa fa-star" aria-hidden="true"></i>
                <i style={{color:'black'}} className="fa fa-star" aria-hidden="true"></i>*/}

                        </div>

                    </InplaceDisplay>
                    <InplaceContent>

                        <Button variant={'info'} onClick={()=>{
                            
                            this.sendRatingRequest();

                        }}>

<small>درخواست نظر سنجی
</small>                  
                        </Button>

                    </InplaceContent>
                </Inplace>

                
                
                </>
        );
    }

    sendRatingRequest() {
        
        
        this.setState({active:false});
        _showMsg("\n" +
            "درخواست نظر سنجی از کاربر ارسال شد")
        MyCaller.Send('AdminSendRatingRequest',{
            customerId:DataHolder.selectedCustomer.Id
        })
    }
}

export default Satistification;




export const ShowStatistification=(props)=>{


    let stars=[];
    for (let i = 1; i <= 6; i++) {
        stars.push(i);
    }
    
    return <>{stars.map((val,i,arr)=>{

            if (i<props.ratingCount.length){
                return                <i key={i} style={{color:'#f8d007'}} className="fa fa-star" aria-hidden="true"></i>

            }else{
                return                 <i key={i} style={{color:'black'}} className="fa fa-star" aria-hidden="true"></i>
                    ;
            }
        })}</>
}