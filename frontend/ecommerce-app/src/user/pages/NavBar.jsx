import axios from 'axios'
import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'

function NavBar() {
  const [userName, setUserName] = useState('')
  const [lastname, setLastName] = useState('')

  useEffect(() => {
    loadUser()
  }, [])

  const loadUser = async () => {
    const id = sessionStorage.getItem('userID')
    if (id) {
      try {
        const response = await axios.get(
          `https://localhost:7253/api/Users/id/${id}`
        )
        if (response.status === 200) {
          setUserName(response.data.firstName)
          setLastName(response.data.lastName)
        } else {
          console.error('Failed to fetch user information')
        }
      } catch (error) {
        console.error('Error fetching user information:', error)
      }
    }
  }

  return (
    <nav className='navbar navbar-expand navbar-dark bg-dark'>
      <div className='container-fluid'>
        <Link className='navbar-brand' to={'/home'}>
          Shoppify
        </Link>
        <button
          className='navbar-toggler'
          type='button'
          data-bs-toggle='collapse'
          data-bs-target='#navbarNav'
          aria-controls='navbarNav'
          aria-expanded='false'
          aria-label='Toggle navigation'
        >
          <span className='navbar-toggler-icon'></span>
        </button>
        <div className='collapse navbar-collapse' id='navbarNav'>
          <ul className='navbar-nav me-auto'>
            <li className='nav-item'>
              <Link className='nav-link' to='/home'>
                Home
              </Link>
            </li>
            <li className='nav-item'>
              <Link className='nav-link' to='/cart'>
                Cart
              </Link>
            </li>
            <li className='nav-item'>
              <Link className='nav-link' to='/order'>
                Order
              </Link>
            </li>
            <li className='nav-item'>
              <Link className='nav-link' to='/login'>
                Logout
              </Link>
            </li>
          </ul>
          <div className='navbar-text ms-auto'>
            <img
              src='https://imagedelivery.net/5MYSbk45M80qAwecrlKzdQ/ce8a56ff-35f2-4609-840f-f78b567b2800/thumbnail?v=2024041210'
              style={{ height: '30px', width: '30px', marginRight: '10px' }}
              alt=''
            />
            <span className='text-white '>
              {userName} {lastname}
            </span>
          </div>
        </div>
      </div>
    </nav>
  )
}

export default NavBar
