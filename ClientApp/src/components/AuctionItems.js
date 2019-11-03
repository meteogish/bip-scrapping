import React, { Component } from 'react';
import { fetchAuctionItems } from '../services/AuctionItemsApi';
import { SearchBar } from './SearchBar';

export class AuctionItems extends Component {

  constructor(props) {
    super(props);
    this.state = { auctionItems: [], loading: false };
  }

  static renderAuctionItems(auctionItems) {

    return auctionItems.map(x => 
      <div>
        <a href={x.link} target="_blank">{x.title}</a>
        <div><em>{x.link}</em></div>
        <p>{new Date(x.uploadDate).toLocaleDateString('pl-PL')}</p>
      </div>)
  }

  filterList = async (searchText, searchDate) => {
    this.setState({ loading: true, ...this.state });
    console.log('searchtext: ', searchText);
    
    const data = await fetchAuctionItems(searchText, searchDate);
    
    this.setState({ auctionItems: data, loading: false });
  }

  render() {
    let mainContent = this.state.loading 
          ? <p><em>Laduje się</em></p>
          : AuctionItems.renderAuctionItems(this.state.auctionItems);

    return (
      <div>
          <SearchBar onChange={this.filterList} />
          <br/>
          <br/>
          <div>
            {mainContent}
          </div>
      </div>
    );
  }
}
