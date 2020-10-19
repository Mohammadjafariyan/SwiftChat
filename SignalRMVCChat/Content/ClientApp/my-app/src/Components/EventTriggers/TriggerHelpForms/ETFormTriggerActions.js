import React, {Component} from 'react';
import Card from "react-bootstrap/Card";
import {MyInput, MySwitcher} from "../../Utilities/Utility";
import {CurrentUserInfo, MyCaller} from "../../../Help/Socket";


class EtFormTriggerActions extends Component {
    state={};

    constructor(props) {
        super(props);
        CurrentUserInfo.EtFormTriggerActions=this;
    }

    render() {
        return (
            <div>
                <Card
                    bg={'light'}
                >
                    <Card.Header>
                        <span className={'float-right'}>
                          عملیات اصلی این Event Trigger 
                        </span>

                        <small className={'float-left text-light'}>
                            میتوانید چند عملیات مختلف تعریف کنید
                        </small>

                    </Card.Header>
                    <Card.Body>
                        {/*<Card.Title>{variant} Card Title </Card.Title>*/}
                        <Card.Text>

                            <MySwitcher title={'نمایش یک پیغام'} name={'IsShowMessageEnabled'} parent={this}>
                                
                                
                            </MySwitcher>
                        </Card.Text>
                    </Card.Body>
                </Card>
            </div>
        );
    }
}

export default EtFormTriggerActions;