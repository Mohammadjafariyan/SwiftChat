import React, {Component} from 'react';
import {MyCaller} from "../../Help/Socket";
import {_showError, _showMsg} from "../../Pages/LayoutPage";
import AutomaticCompaign from "./Automatic/AutomaticCompaign";
import ManualCompaign from "./Manual/ManualCompaign";
import {MyCard, MyFieldset} from "../../Routing/Manage/RoutingSave";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {RadioButton} from "primereact/radiobutton";
import Button from "react-bootstrap/cjs/Button";
import {MyModal} from "../../Components/Modal";
import CompaignLogReceiverTable from "./Logs/CompaignLogReceiverTable";
import {CurrentUserInfo} from "../../CurrentUserInfo";


class CompaignSave extends Component {
    state = {
        compaignType: null
    };

    componentDidMount() {
        CurrentUserInfo.CompaignSave = this;

        this.props.parent.setState({Save: this});

        console.log('RoutingSave->this.props.selected', this.props.selected);
        this.setState({
         
            selected: this.props.selected,
            compaignType: this.props.selected ? this.props.selected.compaignType:null

        });
    }
    
    

    compaignSaveCallback(res) {
        if (this.callback)
            this.callback();

        _showMsg('ذخیره شد')
        this.props.parent.setState({
            selected: null
        });

        CurrentUserInfo.CompaignLayout.setState({changed:false});

    }
    
    
    componentWillUnmount() {

        
        this.props.parent.showSaveModal(this.state.selected,()=>{
            this.state.selected.IsChanged=true;
        });

       // CurrentUserInfo.CompaignIndex.componentDidMount();
    }



    render() {
        if (!this.state.selected) {
            return <></>;
        }


        return (
            <div>

                

                {!this.state.started && <>

                    <MyFieldset title={" انتخاب نوع کمپین " + this.state.selected.Name}>
                        <Row>
                            <Col>
                                <MyCard header={'نوع کمپین'}
                                        title={'اجرای کمپین دستی به دست شماست و کمپین اتوماتیک توسط سیستم با سیاست گذاری شما اجرا می شود'}>
                                    <Row>
                                        <Col>
                                            <RadioButton name="compaignType" value="automatic"
                                                         onChange={(e) => {
                                                            
                                                             this.setState({compaignType: 'automatic'})
                                                             this.state.selected.compaignType = 'automatic';
                                                             this.state.selected.IsAutomatic=true;

                                                         }}
                                                         checked={this.state.compaignType === 'automatic'}/>
                                                         
                                                         <label>کمپین اتوماتیک</label>
                                        </Col>
                                        <Col>

                                            <RadioButton name="compaignType" value="manual"
                                                         onChange={(e) => {
                                                             this.setState({compaignType: 'manual'});

                                                             this.state.selected.compaignType = 'manual';
                                                             this.state.selected.IsAutomatic=false;

                                                         }}
                                                         checked={this.state.compaignType === 'manual'}/>

                                            <label>کمپین ارسال دستی</label>
                                        </Col>
                                    </Row>



                                    <br/>
                                    <Button onClick={() => {

                                        if (!this.state.selected.compaignType){
                                            _showError('لطفا ابتدا نوع کمپینی که مایل هستید ایجاد کنید را انتخاب کنید')
                                            return ;
                                        }

                                        this.setState({started: true})

                                    }}>

                                        شروع
                                    </Button>

                                </MyCard>
                            </Col>
                        </Row>
                              
                        
                    </MyFieldset>
                </>}

                {this.state.started && <>
                    {this.state.compaignType === 'automatic' && <AutomaticCompaign></AutomaticCompaign>}
                    {this.state.compaignType === 'manual' && <ManualCompaign></ManualCompaign>}
                </>}

            </div>
        );
    }

    Save(callback) {


        this.callback = callback;


        _showMsg('در حال ذخیره کمپین')

        MyCaller.Send('CompaignSave', this.state.selected);


    }


}

export default CompaignSave;


export const _GetSelectedCompaign = () => {

    return  CurrentUserInfo.CompaignSave.state.selected;
}


export const _SaveSelectedCompaign = (callback) => {

    return CurrentUserInfo.CompaignSave.Save(callback);
}