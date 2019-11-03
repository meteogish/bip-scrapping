import React, { Component } from 'react';
import { fetch2 } from './FetchApi';
import { SearchBar } from './SearchBar';

export class FetchData extends Component {
  static displayName = FetchData.name;

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

    const params = {
      searchText: searchText,
      fromDate: searchDate.toISOString().substring(0, 10)
    };

    const response = await fetch2('auctionItems', {
      queryParams: params
    });

    const data = await response.json();
    this.setState({ auctionItems: data, loading: false });
  }

  render() {
    let mainContent = this.state.loading 
          ? <p><em>Laduje się</em></p>
          : FetchData.renderAuctionItems(this.state.auctionItems);

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
