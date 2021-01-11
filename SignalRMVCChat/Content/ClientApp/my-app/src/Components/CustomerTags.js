import React, {Component} from "react";
import {MyCaller} from "./../Help/Socket";
import {Badge} from "react-bootstrap";
import {DataHolder} from "../Help/DataHolder";
import {CurrentUserInfo} from "../CurrentUserInfo";

export default class CustomerTags extends Component {


    constructor(props, context) {
        super(props, context);
        this.state = {};

        CurrentUserInfo.CustomerTags = this;
    }


    deleteTagFormUserTagsById(tagId) {
        MyCaller.Send("DeleteTagFormUserTagsById", {
            tagId: tagId,
            customerId: this.props.customer.Id,
            target: this.props.customer.Id
        });
    }

    deleteTagFormUserTagsByIdCallback(res) {
        if (!res.Content.TargetCustomerId) {

            CurrentUserInfo.LayoutPage.showError('TargetCustomerId is null');
            return;
        }
        if (!res.Content.TargetTagId) {

            CurrentUserInfo.LayoutPage.showError('TargetTagId is null');
            return;
        }

        let TargetCustomerId = res.Content.TargetCustomerId;
        let TargetTagId = res.Content.TargetTagId;

        if (DataHolder.selectedCustomer && DataHolder.selectedCustomer.Tags) {
            if (DataHolder.selectedCustomer.Id === TargetCustomerId) {
               CurrentUserInfo.CustomersPage.  GetUserAddedToTags(DataHolder.selectedCustomer.Id)
               /* if (!DataHolder.selectedCustomer.Tags.find(t => t.Id === TargetTagId)) {
                    CurrentUserInfo.LayoutPage.showError('TargetTagId is null');
                    return;
                }
                
                // آپدیت تگ های کاربر انتخاب شده اگر تگ ها از بین تگ های او باشد
                DataHolder.selectedCustomer.Tags=DataHolder.selectedCustomer.Tags.filter(t => t.Id !== TargetTagId).find(t => t.Id === TargetTagId)
                */
            }
        }

        CurrentUserInfo.LayoutPage.showMsg('برچسب با موفقیت برداشته شد');


        // آپدیت لیست کاربران
        CurrentUserInfo.OnlineCustomerListHolder.GetClientsListForAdmin();
    }


    render() {

        let tags;
        if (!this.props.customer ||
            !this.props.customer.Tags ||
            !this.props.customer.Tags || !this.props.customer.Tags.length) {


            if (!this.props.customer ||
                !this.props.customer.CustomerTags ||
                !this.props.customer.CustomerTags || !this.props.customer.CustomerTags.length) {
                return <></>;
            } else {
                tags = this.props.customer.CustomerTags

            }
        } else {
            tags = this.props.customer.Tags
        }
        return (
            <div>
               <TagsShow tags={tags}  parent={this}/>
            </div>
        );
    }
}


export const TagsShow=(props)=>{
    return <>
        {props.tags.map((el, i, arr) => {
            return (
                <>
                    <Badge variant="primary">

                        <i
                            style={{fontSize: "14px"}}
                            className="fa fa-tags"
                            aria-hidden="true"
                        ></i>
                        <span>{el.Name}</span>
                        
                        {props.parent &&   <span
                            style={{color: 'white'}}
                            type="button"
                            onClick={() => {
                                props.parent.deleteTagFormUserTagsById(el.Id);
                            }}
                        >x</span>}
                       
                    </Badge>


                </>
            );
        })}
        </>
}
